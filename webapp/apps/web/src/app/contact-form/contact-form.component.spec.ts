import { ReactiveFormsModule } from '@angular/forms';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactFormComponent } from './contact-form.component';
import { By } from '@angular/platform-browser';

describe('ContactFormComponent', () => {
  let component: ContactFormComponent;
  let fixture: ComponentFixture<ContactFormComponent>;

  function sendInput(inputElement: HTMLInputElement, text: string) {
    inputElement.value = text;
    inputElement.dispatchEvent(new Event('input'));
    fixture.detectChanges();
    return fixture.whenStable();
  }

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContactFormComponent ],
      imports: [
        ReactiveFormsModule
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create an empty form when contact is not set and the save button is disabled', () => {
    component.ngOnInit();
    fixture.detectChanges();

    const firstNameInput = fixture.debugElement.query(By.css('#firstNameInput')).nativeElement;
    expect(firstNameInput.value).toBeFalsy();

    const surnameInput = fixture.debugElement.query(By.css('#surnameInput')).nativeElement;
    expect(surnameInput.value).toBeFalsy();

    const dateOfBirthInput = fixture.debugElement.query(By.css('#dateOfBirthInput')).nativeElement;
    expect(dateOfBirthInput.value).toBeFalsy();

    const emailInput = fixture.debugElement.query(By.css('#emailInput')).nativeElement;
    expect(emailInput.value).toBeFalsy();

    const saveButton = fixture.debugElement.query(By.css('#saveButton')).nativeElement;
    expect(saveButton.disabled).toBeTruthy();
  });

  it('should display validation errors on the form when the input data is invalid', () => {
    component.ngOnInit();
    component.contact = {
      firstName: 'name',
      surname: 'surname',
      dateOfBirth: new Date(1900, 10, 10),
      email: 'one@two.com',
    }
    fixture.detectChanges();

    const dateOfBirthInput = fixture.debugElement.query(By.css('#dateOfBirthInput')).nativeElement;
    sendInput(dateOfBirthInput, new Date().toLocaleDateString()).then(m => {
      const dateOfBirthValidation = fixture.debugElement.query(By.css('#dateOfBirthValidation')).nativeElement;
      expect(dateOfBirthValidation.textContent).toContain('Date of birth should be in the past')
    });

    const emailInput = fixture.debugElement.query(By.css('#emailInput')).nativeElement;
    sendInput(emailInput, "email without domain").then(m => {
      const emailValidation = fixture.debugElement.query(By.css('#emailValidation')).nativeElement;
      expect(emailValidation.textContent).toContain('Email format should be valid')
    });
  });


  it('should create a form when contact is set and the save button is enabled when the data validation passed', () => {
    component.ngOnInit();
    component.contact = {
      firstName: 'name',
      surname: 'surname',
      dateOfBirth: new Date(1900, 10, 10),
      email: 'one@two.com',
    }
    fixture.detectChanges();

    const firstNameInput = fixture.debugElement.query(By.css('#firstNameInput')).nativeElement;
    expect(firstNameInput.value).toBe('name');

    const surnameInput = fixture.debugElement.query(By.css('#surnameInput')).nativeElement;
    expect(surnameInput.value).toBe('surname');

    const dateOfBirthInput = fixture.debugElement.query(By.css('#dateOfBirthInput')).nativeElement;
    expect(dateOfBirthInput.value).toBe('1900-11-10');

    const emailInput = fixture.debugElement.query(By.css('#emailInput')).nativeElement;
    expect(emailInput.value).toBe('one@two.com');

    const saveButton = fixture.debugElement.query(By.css('#saveButton')).nativeElement;
    expect(saveButton.disabled).toBeFalsy();
  });

  it('should raise the save event when the data validation passed and save button is been pushed', () => {
    component.ngOnInit();
    component.contact = {
      firstName: 'name',
      surname: 'surname',
      dateOfBirth: new Date(1900, 10, 10),
      email: 'one@two.com',
    }
    jest.spyOn(component.saveContact, 'emit');
    fixture.detectChanges();

    const saveButton = fixture.debugElement.query(By.css('#saveButton')).nativeElement;
    saveButton.click();

    expect(component.saveContact.emit).toBeCalledTimes(1);
  });
});
