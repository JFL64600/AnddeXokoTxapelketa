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
import { Tournament } from '../libs/tournament';
import { Group } from '../libs/group';
import { Player } from '../libs/player';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { Match } from '../libs/match';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApplyPipe } from 'ngxtension/call-apply';

@Component({
  selector: 'app-root',
  imports: [
    MatExpansionModule,
    MatToolbarModule,
    MatCardModule,
    MatButtonModule,
    MatListModule,
    MatInputModule,
    FormsModule,
    ApplyPipe,
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  #snackBar = inject(MatSnackBar);

  private BoysTableName = 'Boys';
  private GirlsTableName = 'Girls';
  private database = inject(Database);
  public tournament: Tournament = new Tournament('2026');
  private value2: any;
  groups = signal<Group[]>([]);

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
    this.tournament.Groups.push(GirlsGroup1JSON as any);
    this.tournament.Groups.push(GirlsGroup2JSON as any);
    this.tournament.Groups.push(BoysGroup1JSON as any);
    this.tournament.Groups.push(BoysGroup2JSON as any);
    this.tournament.Groups.push(BoysGroup3JSON as any);
    this.tournament.Groups.push(BoysGroup4JSON as any);
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
    this.generateGroups();
  }

  public GetPlayerID(group: Group, playerIDInScore: any): string {
    console.log(playerIDInScore - 1);
    return group.Players[playerIDInScore - 1].toString();
  }

  public GetPlayerNameFromID(group: Group, playerIDInScore: any): string {
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

  isDisabled(match: Match, editedPoints0: number, editedPoints1: number): boolean {
    if (match.Scores[0].EditedPoints === null || match.Scores[1].EditedPoints === null) {
      return true;
    }
    return match.Scores[0].Points == editedPoints0 && match.Scores[1].Points == editedPoints1;
  }

  cancel(match: Match) {
    match.Scores.forEach((score) => {
      score.EditedPoints = score.Points;
    });
  }

  save(match: Match) {
    match.Scores.forEach((score) => {
      score.Points = score.EditedPoints;
    });
    this.#snackBar.open('Puntuazioa gorde da', '', { duration: 2000 });
  }

  private generateGroups() {
    let groups: Group[] = [];
    for (let i = 0; i < this.tournament.Groups.length; i++) {
      const group = this.tournament.Groups[i];
      let players: Player[] = [];
      switch (group.Type) {
        case 0:
          players = this.tournament.Girls.filter((c) => group.Players.includes(c.ID));
          break;
        case 1:
          players = this.tournament.Boys.filter((c) => group.Players.includes(c.ID));
          break;
      }
      const rotations = group.Rotations;
      rotations.forEach((rotation) => {
        rotation.Matches.forEach((match) => {
          match.Scores.forEach((score) => {
            score.EditedPoints = score.Points;
            score.PlayerName = this.GetPlayerNameFromID(group, score.ID);
          });
        });
      });
      groups.push({
        Name: group.Name,
        Type: group.Type,
        Players: players.map((p) => p.ID),
        Rotations: rotations,
      } as Group);
    }
    this.groups.set(groups);
  }
}
