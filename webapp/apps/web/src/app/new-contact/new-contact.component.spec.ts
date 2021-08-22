import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewContactComponent } from './new-contact.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '../core/core.module';
import { Config } from '../core';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ContactFormComponent } from '../contact-form/contact-form.component';

describe('NewContactComponent', () => {
  let component: NewContactComponent;
  let fixture: ComponentFixture<NewContactComponent>;

  beforeEach(async () => {
    global.fetch = jest.fn().mockImplementation(() =>
      Promise.resolve({
        ok: true,
        json: () => { baseUrl: "http://url.com"}
      })
    );

    await TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      declarations: [ NewContactComponent , ContactFormComponent ],
      imports: [
        CoreModule,
        RouterTestingModule,
        ReactiveFormsModule
      ],
      providers: [
        { provide: Config, useValue: { baseUrl: "http://url.com"} },
        { provide: ToastrService, useValue: {success: jest.fn(s => {}), error: jest.fn(s => {})} },
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
