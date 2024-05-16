import { Component, Input, OnInit } from '@angular/core';
import { Video } from '../../../_models/video';
import { NgIf } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-video-tile',
  standalone: true,
  imports: [NgIf, RouterLink, RouterLinkActive],
  templateUrl: './video-tile.component.html',
  styleUrl: './video-tile.component.scss',
})
export class VideoTileComponent implements OnInit {
  @Input() video: Video | undefined;

  constructor() {}

  ngOnInit(): void {}
}
