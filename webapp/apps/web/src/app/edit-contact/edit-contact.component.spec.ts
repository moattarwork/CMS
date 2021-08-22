import { ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NO_ERRORS_SCHEMA } from '@angular/compiler';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditContactComponent } from './edit-contact.component';
import { CoreModule } from '../core/core.module';
import { Config } from '../core';
import { RouterTestingModule } from '@angular/router/testing';
import { ContactFormComponent } from '../contact-form/contact-form.component';



describe('EditContactComponent', () => {
  let component: EditContactComponent;
  let fixture: ComponentFixture<EditContactComponent>;

  beforeEach(async () => {
    global.fetch = jest.fn().mockImplementation(() =>
      Promise.resolve({
        ok: true,
        json: () => { baseUrl: "http://url.com"}
      })
    );

    await TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      declarations: [ EditContactComponent, ContactFormComponent ],
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
    fixture = TestBed.createComponent(EditContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
