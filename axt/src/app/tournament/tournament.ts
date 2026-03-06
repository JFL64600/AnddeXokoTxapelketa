import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { Database, objectVal, ref, update } from '@angular/fire/database';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApplyPipe } from 'ngxtension/call-apply';
import { Observable } from 'rxjs';
import { Group } from '../../libs/group';
import { Match } from '../../libs/match';
import { Player } from '../../libs/player';
import { Tournament } from '../../libs/tournament';

@Component({
  selector: 'axt-tournament',
  imports: [
    MatExpansionModule,
    MatCardModule,
    MatButtonModule,
    MatListModule,
    MatInputModule,
    FormsModule,
    ApplyPipe,
  ],
  templateUrl: './tournament.html',
  styleUrl: './tournament.css',
})
export class TournamentPage implements OnInit {
  #snackBar = inject(MatSnackBar);

  BoysTableName = 'Boys';
  GirlsTableName = 'Girls';
  GroupsTableName = 'Groups';
  private database = inject(Database);
  public tournament = signal<Tournament>(new Tournament('2026'));
  private boysObservable: Observable<unknown>;
  private girlsObservable: Observable<unknown>;
  private groupsObservable: Observable<unknown>;

  constructor() {
    this.boysObservable = objectVal(ref(this.database, this.BoysTableName));
    this.girlsObservable = objectVal(ref(this.database, this.GirlsTableName));
    this.groupsObservable = objectVal(ref(this.database, this.GroupsTableName));

    effect(() => {
      if (this.tournament().Groups !== null && this.tournament().Groups.length > 0) {
        this.tournament().Groups.forEach((group) => {
          const size = group.Players.length;
          const matrix = Array.from({ length: size }, () => Array(size).fill(0));
          group.Rotations.forEach((rotation) => {
            rotation.Matches.forEach((match) => {
              const player1Index = group.Players.findIndex(
                (playerID) => playerID === match.Scores[0].ID,
              );
              const player2Index = group.Players.findIndex(
                (playerID) => playerID === match.Scores[1].ID,
              );
              if (player1Index !== -1 && player2Index !== -1) {
                matrix[player1Index][player2Index] = match.Scores[0].Points;
                matrix[player2Index][player1Index] = match.Scores[1].Points;
              }
            });
          });
          group.Matrix = matrix;
        });
      }
    });
  }

  ngOnInit() {
    this.boysObservable.subscribe((value) => {
      if (value !== null && typeof value === 'object') {
        this.tournament.update((tournament) => ({
          ...tournament,
          Boys: value as Player[],
        }));
      }
    });
    this.girlsObservable.subscribe((value) => {
      if (value !== null && typeof value === 'object') {
        this.tournament.update((tournament) => ({
          ...tournament,
          Girls: value as Player[],
        }));
      }
    });
    this.groupsObservable.subscribe((value) => {
      if (value !== null && typeof value === 'object') {
        const mayBeGroups: Group[] = Object.values(value) as Group[];
        this.tournament.update((tournament) => ({
          ...tournament,
          Groups: mayBeGroups,
        }));
      }
    });
  }

  public getPlayerNameFromID(
    tournament: Tournament,
    group: Group,
    playerIDInScore: number,
  ): string {
    const playerID = group.Players[playerIDInScore - 1];
    switch (group.Type) {
      case 0:
        return tournament?.Girls[playerID - 1]?.Name ?? '';
      case 1:
        return tournament?.Boys[playerID - 1]?.Name ?? '';
      default:
        return '';
    }
  }

  cancel(match: Match) {
    match.Scores.forEach((score) => {
      score.EditedPoints = score.Points;
    });
  }

  save(groupIndex: number, group: Group) {
    update(ref(this.database, `${this.GroupsTableName}/${groupIndex}`), group);
    this.#snackBar.open('Puntuazioa gorde da', '', { duration: 2000 });
  }
}
