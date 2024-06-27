export class AppUser {
    Id: string;
    Name: string;
    Email: string;
    Nickname: string;
    Picture: string;
    Age: number;
    Danceability: number;
    Energy: number;
    Speechiness: number;
    Loudness: number;
    Instrumentalness: number;
    Liveness: number;
    ModifiedAt: Date;
    RequesterId: string;
    IsProfileCompleted : boolean;

    constructor(
        Id: string,
        Name: string,
        Email: string,
        Nickname: string,
        Picture: string,
        Age: number,
        Danceability: number,
        Energy: number,
        Speechiness: number,
        Loudness: number,
        Instrumentalness: number,
        Liveness: number,
        ModifiedAt: Date,
        RequesterId: string,
        IsProfileCompleted : boolean
    ) {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.Nickname = Nickname;
        this.Picture = Picture;
        this.Age = Age;
        this.Danceability = Danceability;
        this.Energy = Energy;
        this.Speechiness = Speechiness;
        this.Loudness = Loudness;
        this.Instrumentalness = Instrumentalness;
        this.Liveness = Liveness;
        this.ModifiedAt = new Date(ModifiedAt);
        this.RequesterId = RequesterId;
        this.IsProfileCompleted = IsProfileCompleted;
    }
}