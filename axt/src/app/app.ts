import { Component } from '@angular/core';
import { TournamentPage } from "./tournament/tournament";
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'axt-root',
  imports: [
        MatToolbarModule,

    TournamentPage
],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App  {
}