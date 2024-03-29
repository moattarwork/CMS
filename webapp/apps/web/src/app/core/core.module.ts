import { APP_INITIALIZER, ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { Config } from './models/config';

export let config: Config;

export const getConfig = (): Config => {
  if (!config) {
    throw new Error('config has not been set or not found');
  }

  return config;
};

export const initConfig = () => async () => {
  const res = await fetch('assets/config.json');
  config = await res.json();
};

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    { provide: Config, useFactory: getConfig },
    { provide: APP_INITIALIZER, useFactory: initConfig, multi: true }
  ]
})
export class CoreModule {
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
    };
  }
}
