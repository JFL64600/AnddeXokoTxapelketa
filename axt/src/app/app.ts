import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Database, objectVal, ref } from '@angular/fire/database';
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
import { Player } from '../libs/player';
import { MatExpansionModule } from '@angular/material/expansion';

@Component({
  selector: 'app-root',
  imports: [MatExpansionModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('axt');
  private BoysTableName = 'Boys';
  private GirlsTableName = 'Girls';
  private database = inject(Database);
  public tournament: Tournament = new Tournament('2026');
  private value2: any;

  constructor() {
    /*
    //this.database = inject(Database);
    //this.tournament = new Tournament("2026");
    //console.log("1");
     objectVal(ref(this.database, this.GirlsTableName)).subscribe((value: any) => {
      //console.log("Value0");
      console.log(value);
      
      Object.keys(value).forEach((key) => {
        this.tournament.Girls.push({
          ID: value[key].ID,
          Name: value[key].Name
        } as Player);
      });
      this.value2 = value;
    });
    */
    //console.log(this.tournament);
    //console.log("Girls-E");
    //console.log(this.tournament.Girls.entries());
    //this.database = getDatabase(inject(FirebaseApp));
    //this.posts$ = objectVal(ref(this.database, this.DATABASE_TABLE_NAME));
  }

  ngOnInit() {
    //console.log("2");
    //console.log(this.value2[0].Name);
    /*
    objectVal(ref(this.database, this.GirlsTableName)).subscribe((value: any) => {
      Object.keys(value).forEach((key) => {
        this.tournament.Girls.push({
          ID: value[key].ID,
          Name: value[key].Name
        } as Player);
      });
    });
    objectVal(ref(this.database, this.BoysTableName)).subscribe((value: any) => {
      console.log("value0");
      console.log(value[0]);
      this.tournament.Boys = value;
      console.log("Boys0");
      console.log(this.tournament.Boys[0]);
      /*
      Object.keys(value).forEach((key) => {
        this.tournament.Boys.push({
          ID: value[key].ID,
          Name: value[key].Name
        } as Player);
      });
    });
    console.log("Boys0-2");
    console.log(this.tournament.Boys[0]);
    //this.tournament.Girls = GirlsPlayersJSON;
    //this.tournament.Boys = BoysPlayersJSON;
    */
    this.tournament.Girls = GirlsPlayersJSON;
    this.tournament.Boys = BoysPlayersJSON;
    this.tournament.Groups.push(GirlsGroup1JSON);
    this.tournament.Groups.push(GirlsGroup2JSON);
    this.tournament.Groups.push(BoysGroup1JSON);
    this.tournament.Groups.push(BoysGroup2JSON);
    this.tournament.Groups.push(BoysGroup3JSON);
    this.tournament.Groups.push(BoysGroup4JSON);
    /*
    //this.tournament.Boys[0].Name = "Toto";

    console.log("Tournoi :");
    console.log(this.tournament);
    this.httpClient.get<any>("../assets/2026/girls.players.json").subscribe((response) => {
      this.tournament.Girls = response;
    });
    this.httpClient.get<any>("../assets/2026/boys.players.json").subscribe((response) => {
      this.tournament.Boys = response;
    });
    console.log(this.tournament);
    this.tournament.Boys[0]['Name'] = "test";
    console.log(this.tournament);
    console.log("Garcons :");
    console.log(this.tournament.Boys);
    this.httpClient.put("../assets/2026/boys.players.json", this.tournament.Boys).subscribe((response) => console.log(response));
    */
  }

  public GetPlayerID(group: Group, playerIDInScore: any): String {
    console.log(playerIDInScore - 1);
    return group.Players[playerIDInScore - 1].toString();
  }

  public GetPlayerNameFromID(group: Group, playerIDInScore: any): String {
    let playerID = group.Players[playerIDInScore - 1].toString();
    switch (group.Type) {
      case 0:
        return this.tournament.Girls.find((c) => c.ID.toString() == playerID)!?.Name;
      case 1:
        return this.tournament.Boys.find((c) => c.ID.toString() == playerID)!?.Name;
      default:
        return '';
    }
  }
}
