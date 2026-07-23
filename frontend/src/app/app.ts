import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NavBar} from "./components/nav-bar/nav-bar";
import  {Main} from "./pages/main/main";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavBar, Main],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend-App');
}
