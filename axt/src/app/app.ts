import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import BoysPlayersJSON from '../assets/2026/boys.players.json';
import BoysGroup1JSON from '../assets/2026/boys.group.1.json';
import BoysGroup2JSON from '../assets/2026/boys.group.2.json';
import BoysGroup3JSON from '../assets/2026/boys.group.3.json';
import BoysGroup4JSON from '../assets/2026/boys.group.4.json';
import GirlsPlayersJSON from '../assets/2026/girls.players.json';
import GirlsGroup1JSON from '../assets/2026/girls.group.1.json';
import GirlsGroup2JSON from '../assets/2026/girls.group.2.json';
import { CommonModule } from '@angular/common';
import { Tournament } from '../libs/tournament';
import { Group } from '../libs/group';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('axt');
  public tournament: Tournament = new Tournament("2026");

  constructor(private httpClient: HttpClient) {}

  ngOnInit() {
    this.tournament.Girls = GirlsPlayersJSON;
    this.tournament.Boys = BoysPlayersJSON;
    this.tournament.Groups.push(GirlsGroup1JSON);
    this.tournament.Groups.push(GirlsGroup2JSON);
    this.tournament.Groups.push(BoysGroup1JSON);
    this.tournament.Groups.push(BoysGroup2JSON);
    this.tournament.Groups.push(BoysGroup3JSON);
    this.tournament.Groups.push(BoysGroup4JSON);
    this.tournament.Boys[0].Name = "Toto";
    console.log("Tournoi :");
    console.log(this.tournament);
    /*
    this.httpClient.get<any>("../assets/2026/girls.players.json").subscribe((response) => {
      this.tournament.Girls = response;
    });
    this.httpClient.get<any>("../assets/2026/boys.players.json").subscribe((response) => {
      this.tournament.Boys = response;
    });
    console.log(this.tournament);
    this.tournament.Boys[0]['Name'] = "test";
    console.log(this.tournament);
    */
    console.log("Garcons :");
    console.log(this.tournament.Boys);
    //this.httpClient.put("../assets/2026/boys.players.json", this.tournament.Boys).subscribe((response) => console.log(response));
  }

   public GetPlayerID(group: Group, playerIDInScore: any): String
   {
    console.log(playerIDInScore - 1)
    return group.Players[playerIDInScore - 1].toString();
   }

  public GetPlayerNameFromID(group: Group, playerIDInScore: any): String
  {
    let playerID = group.Players[playerIDInScore - 1].toString();
    switch (group.Type)
    {
        case 0:
          return this.tournament.Girls.find((c) => c.ID.toString() == playerID)!?.Name;
        case 1:
          return this.tournament.Boys.find((c) => c.ID.toString() == playerID)!?.Name;
        default:
          return "";
    }
  }
}
