import { createAction, props } from '@ngrx/store';
import { Contact } from '../core';
import { ContactRequest } from '../core/models/contact-request';

export const enum ContactActionTypes {
  LoadContacts = '[Contact List] Load Contacts',
  ContactsLoaded = '[Contact List] Load Contacts Success',
  ContactsLoadError = '[Contact List] Load Contacts Error',
  NewContact = '[Contact List] New Contact',
  NewContactSuccess = '[Contact List] New Contact Success',
  NewContactError = '[Contact List] New Contact Error',
  UpdateContact = '[Contact List] Update Contact',
  UpdateContactSuccess = '[Contact List] Update Contact Success',
  UpdateContactError = '[Contact List] Update Contact Error'
}

export const loadContacts = createAction(
  ContactActionTypes.LoadContacts
);

export const contactsLoaded = createAction(
  ContactActionTypes.ContactsLoaded,
  props<{ contacts: Contact[] }>()
);

export const contactsLoadError = createAction(
  ContactActionTypes.ContactsLoaded,
);

export const newContact = createAction(
  ContactActionTypes.NewContact,
  props<{ contact: ContactRequest }>()
);

export const newContactSuccess = createAction(
  ContactActionTypes.NewContactSuccess,
  props<{ contact: Contact }>()
);

export const newContactError = createAction(
  ContactActionTypes.NewContactError
);

export const updateContact = createAction(
  ContactActionTypes.UpdateContact,
  props<{ contact: ContactRequest, index: number }>()
);

export const updateContactSuccess = createAction(
  ContactActionTypes.UpdateContactSuccess,
  props<{ contact: Contact, index: number }>()
);
export const updateContactError = createAction(
  ContactActionTypes.UpdateContactError
);






