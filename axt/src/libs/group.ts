import { Rotation } from './rotation';

export class Group {
  Name = '';
  Type = 0;
  Players: number[] = [];
  Rotations: Rotation[] = [];
  Matrix: number[][] = [];
}
