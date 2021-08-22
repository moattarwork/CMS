import { Component, OnInit } from '@angular/core';
import { Observable, of, pipe } from 'rxjs';
import { ContactService } from '../core/services/contact.service';
import { Contact } from '../core';
import { Router } from '@angular/router';

@Component({
  selector: 'web-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent implements OnInit {
  contactList$: Observable<Contact[]> | undefined = undefined;

  constructor(private contactService: ContactService, private router: Router) { }

  ngOnInit(): void {
    this.contactList$ = this.contactService.getContactList();
  }
}
