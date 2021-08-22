import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ContactService, ContactRequest } from '../core';
import { Router } from '@angular/router';

export function pastDateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const today = new Date();
    const validPastDate = new Date(control.value) < today;
    return validPastDate ? null : {pastDate: {value: control.value}};
  };
}

@Component({
  selector: 'web-new-contact',
  templateUrl: './new-contact.component.html',
  styleUrls: ['./new-contact.component.scss']
})
export class NewContactComponent {
  constructor(private contactService: ContactService,
    private toastr: ToastrService,
    private router: Router) { }

  saveContact(evt: ContactRequest) {
    if (evt){
      this.contactService.newContact(evt).subscribe(
        data => {
          this.toastr.success(`Contact ${data.firstName} ${data.surname} has successfully added.`);
          setTimeout(()=>{
            this.router.navigate(['/']);
          }, 2000);
        },
        err => {
          console.log(err)
           this.toastr.error(err.message)
        })
    }
  }
}
