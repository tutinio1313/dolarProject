import { Component } from '@angular/core';
import {MainDashboardRow} from '../../types/types';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  public collection : undefined | Promise<MainDashboardRow[]> | MainDashboardRow[] = [{name : "crypto", buyPrice : 1370, sellPrice:1430, lastUpdate:"2026-05-24T14:30:00"}];
  public isArray : boolean = Array.isArray(this.collection);

  protected readonly Array = Array;
}
