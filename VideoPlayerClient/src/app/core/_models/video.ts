import { User } from './user';

export interface Video {
  id: string;
  title: string;
  description: string;
  videoUrl: string;
  thumbnailUrl: string;
  uploadDate: string;
  duration: string;
  uploadDateInfo: string;
  uploadedBy: User;
  views: number;
  likes: number;
  dislikes: number;
}

export interface UploadVideo {
  title: string;
  description: string;
  videoData: VideoData;
  requestedById: string;
}

export interface VideoData {
  videoType: string;
  bytes: string;
}
