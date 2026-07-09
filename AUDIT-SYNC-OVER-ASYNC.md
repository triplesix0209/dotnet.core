# Báo cáo audit & fix: Sync-over-Async — TripleSix.Core

- **Ngày:** 2026-07-09
- **Branch:** `feature/v9`
- **Phạm vi:** toàn bộ solution (201 file C#, loại trừ `bin/`, `obj/`)
- **Phương pháp:** quét pattern + workflow audit 5 góc nhìn (42 agent, mỗi finding được verify phản biện độc lập) + code review 2 vòng (csharp-reviewer, verdict cuối: **approve**)
- **Kết quả build sau fix:** 0 error, không phát sinh warning StyleCop/nullable mới

## Tóm tắt

| # | File | Mức độ | Lỗi | Trạng thái |
|---|------|--------|-----|------------|
| 1 | `Core/AutofacModules/ServiceInterceptor.cs` | **CRITICAL** | `Wait()` block mọi async service method toàn framework | ✅ Đã fix |
| 2 | `Core/AutofacModules/ServiceInterceptor.cs` | HIGH | Nuốt exception của method sync (silent failure) | ✅ Đã fix |
| 3 | `Core/Identity/IdentitySecurityTokenHandler.cs` | HIGH | Fetch JWKS bằng HTTP blocking trong luồng xác thực | ✅ Đã fix |
| 4 | `Core/Identity/IdentitySecurityTokenHandler.cs` | MEDIUM | `new HttpClient()` mỗi lần fetch (nguy cơ socket exhaustion) | ✅ Đã fix |
| 5 | `Core/Identity/IdentitySecurityTokenHandler.cs` | MEDIUM | Cache signing key là `Dictionary` static không khoá (race condition) | ✅ Đã fix |
| 6 | `Core/Identity/IdentitySecurityTokenHandler.cs` | MEDIUM | Cache stampede: N request cùng fetch JWKS khi cache hết hạn | ✅ Đã fix |
| 7 | `Core/Identity/IdentitySecurityTokenHandler.cs` | LOW | Không có timeout riêng cho HTTP JWKS (mặc định 100s) | ✅ Đã fix |
| 8 | `Core/Services/BaseService.Entity.cs` (7 chỗ) | HIGH | `task!.Wait()` trong method đã async (có 2 chỗ nằm trong vòng lặp) | ✅ Đã fix |
| 9 | `Core/Services/StrongService.cs` (2 chỗ) | MEDIUM | `Task.WaitAll(task!)` trong method đã async | ✅ Đã fix |
| 10 | `Core/WebApi/Extension.cs` | MEDIUM | Async lambda gán vào delegate `Action` của OpenTelemetry (async void) | ✅ Đã fix |
| 11 | `Core/AutofacModules/ServiceInterceptor.cs` | HIGH | *(phát sinh từ fix #1)* Rò rỉ `Activity.Current` làm gãy cây trace | ✅ Đã fix |
| 12 | `Sample/WebApi/Startup.cs` | MEDIUM | SQL sync trong callback `GetSigningKeyMethod` (Dynamic mode) | ⏳ Chờ quyết định (đổi public API) |

---

## Chi tiết từng lỗi

### 1. ServiceInterceptor block mọi async service method — CRITICAL

**Vị trí (trước fix):** `Core/AutofacModules/ServiceInterceptor.cs`, method `Intercept`.

```csharp
invocation.Proceed();
if (invocation.ReturnValue is Task taskResult)
    taskResult.Wait();   // block thread đến khi task xong
```

**Vấn đề:** Interceptor này được Autofac gắn cho **mọi `IService`** qua `RegisterAllService` (`Core/AutofacModules/Extension.cs`). Mọi method async của toàn bộ service layer bị chạy đồng bộ: thread request đứng chờ task xong rồi mới trả về Task (đã hoàn tất) cho caller `await`. Toàn bộ lợi ích async mất sạch; dưới tải cao gây thread-pool starvation. Lý do `Wait()` tồn tại: giữ `Activity` span (tracing) bao trọn thời gian chạy method.

**Fix:** Không block nữa. Nếu method trả `Task`/`Task<T>`, thay `ReturnValue` bằng task wrapper: `await` task gốc, tag lỗi lên activity nếu fault, dispose activity trong `finally` → span vẫn đo đúng trọn thời gian chạy async. `Task<T>` được bọc qua `WrapWithResult<T>` tạo bằng `MakeGenericMethod` (có cache theo kiểu `TResult` vì đây là hot path).

### 2. Interceptor nuốt exception của method sync — HIGH

**Vấn đề:** Cả 3 khối `catch` cũ chỉ tag lỗi lên activity rồi kết thúc, không `throw` lại. Method **sync** ném exception → caller nhận `null`/default mà không biết có lỗi (silent failure). (Với method async, exception vẫn tới caller qua faulted task nên không lộ ra.)

**Fix:** Catch → tag → dispose activity → **`throw`** lại. Đồng thời exception từ async path giờ trả **nguyên gốc** (do `await` thay vì `Wait()`), không còn bị bọc `AggregateException` — `catch (NotFoundException)` phía caller bắt được đúng như kỳ vọng.

### 3. Fetch JWKS bằng HTTP blocking trong luồng xác thực — HIGH

**Vị trí (trước fix):** `Core/Identity/IdentitySecurityTokenHandler.cs`, case `Jwks` trong `ValidateToken`.

```csharp
using var httpClient = new HttpClient();
var jwksResponse = httpClient.GetStringAsync(Setting.JwksEndpoint).GetAwaiter().GetResult();
```

**Vấn đề:** Handler chạy trên đường xác thực của mọi request có JWT. Mỗi khi cache hết hạn (`SigningKeyCacheTimelife`), thread request bị block chờ network I/O → spike latency dây chuyền dưới tải.

**Nguyên nhân gốc & cách fix:** `ValidateToken` (API của `ISecurityTokenValidator`) chỉ có bản sync nên không `await` được bên trong. Tuy nhiên handler được đăng ký qua `options.TokenHandlers` (`Core/WebApi/Extension.cs`), và JwtBearer trên .NET 8/9 **luôn gọi `ValidateTokenAsync`** (đã xác minh bằng decompile `Microsoft.AspNetCore.Authentication.JwtBearer` 9.0.17: nhánh `TokenHandlers` gọi `await tokenHandler.ValidateTokenAsync(...)`; đường `SecurityTokenValidators` sync đã bị đánh dấu `[Obsolete]`). Bản `ValidateTokenAsync` kế thừa từ `JwtSecurityTokenHandler` 8.16.0 chỉ là wrapper sync-giả (`Task.FromResult` bọc quanh lời gọi virtual `this.ValidateToken`).

→ **Fix:** override `ValidateTokenAsync`: fetch JWKS bằng `await` thật và nạp cache (pre-warm), sau đó gọi `base.ValidateTokenAsync` → chuỗi validate cũ chạy y nguyên nhưng cache đã ấm, không còn HTTP block. Đường sync giữ `GetAwaiter().GetResult()` làm **fallback legacy** (giới hạn API, thực tế gần như không chạy vì request luôn đi qua bản async).

### 4. `new HttpClient()` mỗi lần fetch — MEDIUM

**Vấn đề:** Tạo rồi dispose client mỗi lần gọi để lại socket TIME_WAIT, tích tụ gây cạn socket trên server chạy lâu.

**Fix:** Một `HttpClient` static dùng chung, cấu hình `SocketsHttpHandler.PooledConnectionLifetime = 5 phút` (tránh kẹt DNS cũ khi endpoint đổi IP).

### 5. Cache signing key không thread-safe — MEDIUM

**Vấn đề:** `static Dictionary<string, SigningKeyCacheItem>` được đọc/ghi đồng thời từ nhiều request thread không có khoá — `Dictionary` thường không an toàn khi ghi song song (có thể ném exception hoặc hỏng cấu trúc nội bộ).

**Fix:** Chuyển sang `ConcurrentDictionary`, đọc bằng `TryGetValue` (helper `GetValidCacheItem` kèm kiểm tra hạn).

### 6. Cache stampede khi hết hạn — MEDIUM

**Vấn đề:** Đúng lúc cache expire, N request cùng thấy miss → N cuộc gọi HTTP song song đập vào identity provider trong khi chỉ cần 1.

**Fix:** Double-checked locking bằng `SemaphoreSlim(1,1)` trong đường async: check ngoài khoá → `await WaitAsync()` → check lại trong khoá → chỉ request đầu fetch, các request khác chờ rồi dùng cache; `Release()` trong `finally`.

### 7. Không có timeout cho HTTP JWKS — LOW

**Vấn đề:** `HttpClient.Timeout` mặc định 100 giây — JWKS endpoint chậm có thể treo xác thực rất lâu.

**Fix:** `Timeout = 10s` trên client dùng chung.

### 8–9. `task!.Wait()` / `Task.WaitAll` trong service base — HIGH/MEDIUM

**Vị trí (trước fix):**

- `Core/Services/BaseService.Entity.cs`: 7 chỗ trong `Create<TResult>`, `CreateWithMapper<TResult>`, `Update<TResult>`, `UpdateWithMapper<TResult>`, `GetFirstOrDefault<TResult>`, `GetList<TResult>`, `GetPage<TResult>`
- `Core/Services/StrongService.cs`: 2 chỗ trong `SoftDelete<TResult>`, `Restore<TResult>`

```csharp
var task = mapMethod.Invoke(result, [ServiceProvider, entity]) as Task;
task!.Wait();          // hoặc Task.WaitAll(task!)
```

**Vấn đề:** Block chờ task `FromEntity` (mapping DTO) **trong method vốn đã `async`** — không có lý do gì để không `await`. Nặng nhất ở `GetList<TResult>` và `GetPage<TResult>` vì `Wait()` nằm trong `foreach` — block thread cho từng entity. Ngoài block, `Wait()` còn bọc exception mapping vào `AggregateException`, phá vỡ semantics catch theo kiểu ở exception middleware. (`FromEntity` là extension point public — DTO ở các app tiêu thụ có thể override thành async thật bất kỳ lúc nào.)

**Fix:** Cả 9 chỗ đổi thành `await task!;`.

### 10. Async void lambda trong OpenTelemetry enrich — MEDIUM

**Vị trí (trước fix):** `Core/WebApi/Extension.cs`, `SetupOpenTelemetry` → `AddHttpClientInstrumentation`.

```csharp
o.EnrichWithHttpRequestMessage = async (activity, requestMessage) => { ... await requestMessage.ToCurl() ... };
```

**Vấn đề:** `EnrichWithHttpRequestMessage` có kiểu `Action<Activity, HttpRequestMessage>` — gán async lambda vào `Action` tạo ra **async void**: (a) exception ngoài `try/catch` sẽ crash process; (b) phần sau `await` chạy đua với vòng đời activity — tag `http.curl` có thể được set sau khi activity đã export (mất tag), và đọc content stream song song với việc HttpClient đang gửi request.

**Fix:** Chuyển lambda về sync; gọi `ToCurl()` và **chỉ đọc `.Result` khi `IsCompletedSuccessfully`** (content dạng buffer như `StringContent`/`ByteArrayContent` hoàn thành đồng bộ — trường hợp phổ biến). Content chưa buffer thì bỏ tag thay vì block/race — trade-off bắt buộc do delegate sync của OpenTelemetry.

### 11. Rò rỉ `Activity.Current` (phát sinh từ fix #1) — HIGH

**Vấn đề:** Sau khi bỏ `Wait()`, `Intercept` trả về **trước khi** task xong → activity của interceptor vẫn là `Activity.Current` ở context caller; lệnh `Dispose` nằm trong async wrapper không truyền ngược ra được (ExecutionContext bị cô lập). Hậu quả: các span kế tiếp trong cùng request nhận nhầm parent là span đã kết thúc → gãy quan hệ cha-con của distributed trace. *(Lỗi do reviewer phát hiện và xác nhận bằng repro thực nghiệm trên .NET 9 — xảy ra vô điều kiện, kể cả khi task hoàn thành đồng bộ.)*

**Fix:** Capture `parentActivity = Activity.Current` trước khi `StartActivity`; sau khi gán wrapper vào `ReturnValue`, khôi phục `Activity.Current = parentActivity` ngay trong frame sync của `Intercept` trước khi return.

### 12. SQL sync trong callback lấy signing key — MEDIUM — ⏳ CHƯA FIX

**Vị trí:** `Sample/WebApi/Startup.cs` (method `GetSigningKey`: `SqlConnection.Open()` + `ExecuteReader()`), truyền vào `AddJwtAccessToken` làm `GetSigningKeyMethod`.

**Vấn đề:** Delegate `GetSigningKeyMethod` có kiểu **sync** `Func<IdentityAppsetting, JwtSecurityToken, string?>` — app nào truyền hàm chạy SQL thì SQL đó chạy đồng bộ trong luồng xác thực mỗi khi cache Dynamic hết hạn.

**Hướng fix đề xuất (cần quyết định vì đổi public API của framework):** thêm delegate async `Func<..., Task<string?>>` (`GetSigningKeyMethodAsync`), pre-warm trong `ValidateTokenAsync` tương tự JWKS, giữ delegate sync cũ để không breaking; cập nhật Sample dùng `OpenAsync`/`ExecuteReaderAsync`.

---

## Thay đổi hành vi cần ghi release note

1. **Task từ `IService` không còn hoàn thành sẵn khi trả về.** Code ở app tiêu thụ gọi service mà quên `await` trước đây vô tình vẫn chạy tuần tự (do interceptor block), giờ chạy song song thật — đúng semantics async, nhưng cần rà nếu có chỗ dựa vào hành vi cũ.
2. **Exception từ method sync không còn bị interceptor nuốt** — trước trả `null` im lặng, giờ ném đúng exception (đây là bug fix).
3. **Exception từ mapping/service trả nguyên gốc** thay vì `AggregateException`.

## Ghi chú còn lại (ngoài phạm vi, nên xử lý đợt riêng)

- Interceptor chỉ xử lý `Task`/`Task<T>`; method trả `ValueTask` sẽ không được trace-await (hiện **không có** member `IService` nào dùng `ValueTask` — dormant).
- NuGet vulnerability có sẵn từ trước: **AutoMapper 14.0.0 (HIGH — GHSA-rvv3-g6hj-g44x)**, OpenTelemetry.Exporter.OpenTelemetryProtocol 1.15.0 (3 advisory moderate) → nên nâng version.
