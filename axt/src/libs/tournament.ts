import { Group } from './group';
import { Player } from './player';

export class Tournament {
  Name: string = '';
  Girls: Player[] = [];
  Boys: Player[] = [];
  Groups: Group[] = [];

  constructor(name: string) {
    this.Name = name;
  }
}
