import { Song } from "./song";

export class Playlist {
    Id : string;
    Name : string;
    UserId : string;
    SongIds: string[];
    Songs : Song[];

    constructor(id: string, name: string, userId: string, songs: string[]) {
        this.Id = id;
        this.Name = name;
        this.UserId = userId;
        this.SongIds = songs;
        this.Songs = [];
    }
}