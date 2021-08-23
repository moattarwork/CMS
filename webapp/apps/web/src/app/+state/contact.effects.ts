import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { ContactService } from '../core';
import { ContactActionTypes, contactsLoaded, contactsLoadError, newContactError, newContactSuccess, updateContactError, updateContactSuccess } from './contact.actions';

import {mergeMap, map, catchError} from 'rxjs/operators'
import { of } from 'rxjs';


@Injectable()
export class ContactEffects {
  loadContacts$ = createEffect(() => this.actions$.pipe(
    ofType(ContactActionTypes.LoadContacts),
    mergeMap(() => this.contactService.getContactList().pipe(
        map(contacts => contactsLoaded({contacts: contacts})),
        catchError(() => of(contactsLoadError()))
    ))
  ));

  newContact$ = createEffect(() => this.actions$.pipe(
    ofType(ContactActionTypes.NewContact),
    mergeMap((action) => this.contactService.newContact(action.contact).pipe(
        map(contact => newContactSuccess({contact: contact})),
        catchError(() => of(newContactError()))
      ))
  ));

  updateContacts$ = createEffect(() => this.actions$.pipe(
    ofType(ContactActionTypes.UpdateContact),
    mergeMap((action) => this.contactService.updateContact(action.contact).pipe(
        map(contact => updateContactSuccess({contact: contact, index: action.index })),
        catchError(() => of(updateContactError()))
      ))
  ));

  constructor(private actions$: Actions, private contactService: ContactService) { }
}
