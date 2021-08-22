import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Contact } from '../models';
import { ConfigService } from './config.service';
import { ContactRequest } from '../models';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  baseUrl: string | undefined = ""

  constructor(private httpClient: HttpClient, private configService: ConfigService) {
    this.baseUrl = this.configService.getConfig().baseUrl;
   }

  getContactList() : Observable<Contact[]> {
    const url = `${this.baseUrl}/api/contacts`;
    return this.httpClient.get<Contact[]>(url);
  }

  getContact(id: string) : Observable<Contact> {
    const url = `${this.baseUrl}/api/contacts/${id}`;
    return this.httpClient.get<Contact>(url);
  }

  newContact(contactRequest: ContactRequest) : Observable<Contact> {
    const url = `${this.baseUrl}/api/contacts`;
    return this.httpClient.post<Contact>(url, contactRequest);
  }

  updateContact(contactId: string, contactRequest: ContactRequest) : Observable<Contact> {
    const url = `${this.baseUrl}/api/contacts/${contactId}`;
    return this.httpClient.put<Contact>(url, contactRequest);
  }
}
