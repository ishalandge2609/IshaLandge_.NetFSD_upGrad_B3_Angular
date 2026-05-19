import {
  ComponentFixture,
  TestBed
} from '@angular/core/testing';

import { PageNotFoundComponent } from './page-not-found.component';

import { provideRouter } from '@angular/router';

import { By } from '@angular/platform-browser';

describe('PageNotFoundComponent', () => {

  let component: PageNotFoundComponent;

  let fixture: ComponentFixture<PageNotFoundComponent>;

  beforeEach(async () => {

    await TestBed.configureTestingModule({
      imports: [
        PageNotFoundComponent
      ],
      providers: [
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(
      PageNotFoundComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should display 404 message', () => {

    const heading =
      fixture.debugElement.query(
        By.css('h4')
      );

    expect(
      heading.nativeElement.textContent
    ).toContain(
      '404 - Page Not Found'
    );

  });

  it('should display error description', () => {

    const paragraph =
      fixture.debugElement.query(
        By.css('p')
      );

    expect(
      paragraph.nativeElement.textContent
    ).toContain(
      "Oops! The page you're looking for doesn't exist."
    );

  });

  it('should have a home button', () => {

    const homeButton =
      fixture.debugElement.query(
        By.css('a.btn-primary')
      );

    expect(homeButton)
      .toBeTruthy();

    expect(
      homeButton.nativeElement.textContent
    ).toContain(
      'Go Back Home'
    );

  });

  it('should have routerLink to home', () => {

    const homeButton =
      fixture.debugElement.query(
        By.css('a.btn-primary')
      );

    expect(
      homeButton.attributes['ng-reflect-router-link']
    ).toContain('/home');

  });

});