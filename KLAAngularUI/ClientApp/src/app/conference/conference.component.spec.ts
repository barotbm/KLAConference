/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ConferenceComponent } from './conference.component';

let component: ConferenceComponent;
let fixture: ComponentFixture<ConferenceComponent>;

describe('Conference component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ConferenceComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ConferenceComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});