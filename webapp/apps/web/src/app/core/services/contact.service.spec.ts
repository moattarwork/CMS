import { TestBed } from '@angular/core/testing';

import { ContactService } from './contact.service';
import { ConfigService } from './config.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Config } from '../models';

describe('ContactService', () => {
  let service: ContactService;
  let config: ConfigService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        { provide: Config, useValue: { baseUrl: "http://url.com"} }
      ]
    });
    service = TestBed.inject(ContactService);
    config = TestBed.inject(ConfigService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
