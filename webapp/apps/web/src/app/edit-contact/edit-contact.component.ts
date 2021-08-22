import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ContactService, ContactRequest, Contact } from '../core';

@Component({
  selector: 'web-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.scss']
})
export class EditContactComponent implements OnInit {
  contact: Contact | undefined

  constructor(private contactService: ContactService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.contactService.getContact(id).subscribe(
        data => { this.contact = data; },
        err => { this.toastr.error(err.message) }
      );
    });

  }

  saveContact(evt: ContactRequest) {
    if (evt && this.contact){
        this.contactService.updateContact(this.contact.contactId, evt).subscribe(
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
