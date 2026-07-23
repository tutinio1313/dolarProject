import { Component } from '@angular/core';
import {Dashboard} from '../../components/dashboard/dashboard';

@Component({
  selector: 'app-main',
  imports: [Dashboard],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {

}
