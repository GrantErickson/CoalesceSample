import type Vue from "vue";

import type { ComponentOptions, AsyncComponent } from "vue";

type Component = ComponentOptions<Vue> | typeof Vue | AsyncComponent;

export default interface IComponent {
  component: Component;

  props: any;

  template?: any;
}
