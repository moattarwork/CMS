import { TestBed } from '@angular/core/testing';
import { Config } from '../models';

import { ConfigService } from './config.service';

describe('ConfigService', () => {
  let service: ConfigService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: Config, useValue: { baseUrl: "http://url.com"} }
      ],
    });
    service = TestBed.inject(ConfigService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return the config correctly', () => {
    const config = service.getConfig()
    expect(config.baseUrl).toBe("http://url.com");
  })
});
