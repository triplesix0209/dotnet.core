import Vue from "vue";
import VueMoment from "vue-moment";
import Moment from "moment-timezone";
import "moment/locale/vi";

Vue.use(VueMoment, {
	moment: Moment.tz.setDefault(Moment.tz.guess()),
});
