#pragma warning disable SA1649 // File name should match first type name

using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Services;

namespace TripleSix.Core.AutoAdmin
{
    public interface IObjectLogService : IService
    {
        /// <summary>
        /// Ghi nhận log object.
        /// </summary>
        /// <typeparam name="TObject">Loại dữ liệu.</typeparam>
        /// <param name="objectId">Id định danh, mặc định sẽ thử lấy field Id của đối tượng.</param>
        /// <param name="newValue">Giá trị ghi nhận.</param>
        /// <param name="oldValue">Giá trị mới.</param>
        /// <param name="objectType">Loại object, mặc định sẽ lấy theo tên của đối tượng.</param>
        /// <param name="note">Ghi chú.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Entity đã được tạo.</returns>
        Task WriteLog<TObject>(
            Guid objectId,
            TObject newValue,
            TObject? oldValue = null,
            string? objectType = null,
            string? note = null,
            CancellationToken cancellationToken = default)
            where TObject : class;

        /// <summary>
        /// Ghi nhận log object sau khi hàm.
        /// </summary>
        /// <typeparam name="TObject">Loại dữ liệu.</typeparam>
        /// <param name="objectId">Id định danh, mặc định sẽ thử lấy field Id của đối tượng.</param>
        /// <param name="oldValue">Giá trị cũ.</param>
        /// <param name="action">Tham tác xử lý làm thay đổi dữ liệu.</param>
        /// <param name="objectType">Loại object, mặc định sẽ lấy theo tên của đối tượng.</param>
        /// <param name="note">Ghi chú.</param>
        /// <param name="cancellationToken">Token để cancel task.</param>
        /// <returns>Entity đã được tạo.</returns>
        Task LogAction<TObject>(
            Guid objectId,
            TObject? oldValue,
            Func<Task<TObject>> action,
            string? objectType = null,
            string? note = null,
            CancellationToken cancellationToken = default)
            where TObject : class;
    }

    public class BaseObjectLogService : BaseService, IObjectLogService
    {
        private static string[] _skipProperties = new[]
        {
            nameof(IIdentifiableEntity.Id),
            nameof(ICreateAuditableEntity.CreateDateTime),
            nameof(ICreateAuditableEntity.CreatorId),
            nameof(IUpdateAuditableEntity.UpdateDateTime),
            nameof(IUpdateAuditableEntity.UpdatorId),
        };

        public IObjectLogDbContext Db { get; set; }

        /// <inheritdoc/>
        public virtual async Task WriteLog<TObject>(
            Guid objectId,
            TObject newValue,
            TObject? oldValue = null,
            string? objectType = null,
            string? note = null,
            CancellationToken cancellationToken = default)
            where TObject : class
        {
            var type = typeof(TObject);
            if (objectType.IsNullOrEmpty()) objectType = type.Name;

            var log = new ObjectLog
            {
                ObjectId = objectId,
                ObjectType = objectType,
                Note = note,
                Fields = new List<ObjectLogField>(),
            };

            foreach (var property in type.GetProperties())
            {
                if (_skipProperties.Contains(property.Name)) continue;
                var newPropertyValue = property.GetValue(newValue).ToJson();
                if (newPropertyValue == null) continue;
                var oldPropertyValue = oldValue == null ? null : property.GetValue(oldValue).ToJson();
                if (oldPropertyValue != null && oldPropertyValue == newPropertyValue) continue;

                log.Fields.Add(new ObjectLogField
                {
                    FieldName = property.Name,
                    NewValue = newPropertyValue,
                    OldValue = oldPropertyValue,
                });
            }

            if (log.Fields.Count == 0) return;

            Db.ObjectLog.Add(log);
            await Db.SaveChangesAsync(true, cancellationToken);
        }

        public virtual async Task LogAction<TObject>(
            Guid objectId,
            TObject? oldValue,
            Func<Task<TObject>> action,
            string? objectType = null,
            string? note = null,
            CancellationToken cancellationToken = default)
            where TObject : class
        {
            oldValue = oldValue.ToJson()
                !.ToObject<TObject>();

            var task = action();
            await task.WaitAsync(cancellationToken);
            var newValue = task.Result;

            if (oldValue == null || newValue == null) return;
            try
            {
                await WriteLog(objectId, newValue, oldValue, objectType, note, cancellationToken);
            }
            catch
            {
            }
        }
    }
}
