import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { ContactListComponent } from './contact-list/contact-list.component';
import { ContactFormComponent } from './contact-form/contact-form.component';
import { NewContactComponent } from './new-contact/new-contact.component';
import { EditContactComponent } from './edit-contact/edit-contact.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from './core/core.module';
import { AppRoutingModule } from './app.routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [AppComponent, ContactListComponent, NewContactComponent, EditContactComponent, ContactFormComponent],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    CoreModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot()],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
