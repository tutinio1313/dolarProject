import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangeRow } from './exchange-row';

describe('ExchangeRow', () => {
  let component: ExchangeRow;
  let fixture: ComponentFixture<ExchangeRow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExchangeRow]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExchangeRow);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
