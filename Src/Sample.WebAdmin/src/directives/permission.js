import Vue from "vue";
import PermissionService from "@/services/permission";

function displayElement(el, binding, vnode) {
	let parentElement = vnode.elm.permissionParentElement;
	if (!parentElement) {
		parentElement = vnode.elm.parentElement;
		vnode.elm.permissionParentElement = parentElement;
	}

	let check = PermissionService.check(binding.value);
	if (check && !parentElement.contains(vnode.elm))
		parentElement.appendChild(vnode.elm);
	else if (!check && parentElement.contains(vnode.elm))
		parentElement.removeChild(vnode.elm);
}

Vue.directive("permission", {
	inserted: displayElement,
	update: displayElement,
});
