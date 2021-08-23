import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ContactRequest } from '../core';
import { formatDate } from '@angular/common'

export function pastDateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate())
    const validPastDate = new Date(control.value) < today;
    return validPastDate ? null : {pastDate: {value: control.value}};
  };
}

@Component({
  selector: 'web-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent implements OnInit {
  contactForm: FormGroup = this.formBuilder.group({});

  private _contact : ContactRequest | undefined;

  @Input()
  public get contact() : ContactRequest | undefined {
    return this._contact;
  }
  public set contact(contact : ContactRequest | undefined) {
    this._contact = contact;
    if (contact)
      this.setValue(contact);
  }

  @Output() saveContact = new EventEmitter<ContactRequest>();

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.contactForm = this.createForm();
  }

  save() {
    if (this.saveContact && this.contactForm && this.contactForm.valid)
      this.saveContact.emit(this.contactForm.value)
  }

  get firstName() { return this.contactForm.get('firstName'); }

  get surname() { return this.contactForm.get('surname'); }

  get dateOfBirth() { return this.contactForm.get('dateOfBirth'); }

  get email() { return this.contactForm.get('email'); }

  private setValue(contact: ContactRequest) {
    this.contactForm.patchValue({
        firstName: contact.firstName,
        surname: contact.surname,
        dateOfBirth: formatDate(contact.dateOfBirth, 'yyyy-MM-dd', 'en'),
        email: contact.email
      });
  }

  private createForm(): FormGroup{
    return this.formBuilder.group({
        firstName: [null, [Validators.required, Validators.maxLength(100)]],
        surname: [null, [Validators.required, Validators.maxLength(100)]],
        dateOfBirth: [new Date(), [Validators.required, pastDateValidator()]],
        email: [null, [Validators.required, Validators.email]],
    })
  }
}
