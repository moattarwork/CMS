import { createReducer, on } from '@ngrx/store';
import { Contact } from '../core';
import { contactsLoaded, newContactSuccess, updateContactSuccess } from './contact.actions';


export const contactFeatureKey = 'contact';

export interface State {
  contacts: Array<Contact>
}

export const initialState: State = {
  contacts: []
};


export const reducer = createReducer(
  initialState,
  on(contactsLoaded, (state, action) => ({ ...state, contacts: action.contacts })),
  on(newContactSuccess, (state, action) => ({ ...state, contacts: [...state.contacts, action.contact] })),
  on(updateContactSuccess, (state, action) => ({
    ...state,
    contacts: [
        ...state.contacts.slice(0, action.index),
        action.contact,
        ...state.contacts.slice(action.index + 1)
    ]
  }))
);

