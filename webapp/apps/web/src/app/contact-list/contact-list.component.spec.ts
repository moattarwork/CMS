import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { ContactListComponent } from './contact-list.component';
import { Config } from '../core';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';

describe('ContactListComponent', () => {
  let component: ContactListComponent;
  let fixture: ComponentFixture<ContactListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContactListComponent ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule
      ],
      providers: [
        { provide: Config, useValue: { baseUrl: "http://url.com"} }
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('display the new contact button', () => {
    const newButton = fixture.debugElement.query(By.css('#newButton')).nativeElement;
    expect(newButton).toBeTruthy();
  });

  it('display list with the current columns', () => {
    const columnList = fixture.debugElement.queryAll(By.css('#contactList > thead > tr > th'));
    expect(columnList.length).toBe(4);

    expect(columnList[0].nativeElement.textContent).toContain("Name");
    expect(columnList[1].nativeElement.textContent).toContain("Date Of Birth");
    expect(columnList[2].nativeElement.textContent).toContain("Email");
    expect(columnList[3].nativeElement.textContent).toContain("Actions");
  });

  it('display empty list when the contact list is empty', () => {
    const contactList = fixture.debugElement.queryAll(By.css('#contactList > tbody > tr'));
    expect(contactList.length).toBe(0);
  });

  it('display correct list when the contact list is presented with 2 contacts', () => {
    const contacts = [
      { contactId: 'c73cb996-df82-4c4c-802e-d379b3033669', firstName: "Name #1", surname: "Surname #1", dateOfBirth: new Date('2020,10,1'), email: '1@domain.com'},
      { contactId: 'b64090ba-b42c-4779-9bc8-524d61b07c9e', firstName: "Name #2", surname: "Surname #2", dateOfBirth: new Date('2020,10,2'), email: '2@domain.com'}
    ]

    component.contactList$ = of(contacts)
    fixture.detectChanges();

    const contactList = fixture.debugElement.queryAll(By.css('#contactList > tbody > tr'));
    expect(contactList.length).toBe(2);

    contactList.forEach((el , index)=> {
      const cells = el.queryAll(By.css('td'));
      const contact = contacts[index];


      expect(cells[0].nativeElement.textContent).toContain(`${contact.firstName} ${contact.surname}`);
      expect(cells[1].nativeElement.textContent).toContain(`0${index+1}/10/2020`);
      expect(cells[2].nativeElement.textContent).toContain(`${index+1}@domain.com`);

      expect(cells[3].query(By.css("Button")).nativeElement).toBeTruthy();
    });
  });

});
