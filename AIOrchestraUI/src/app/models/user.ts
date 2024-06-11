export class AppUser {
    Id: string;
    Name: string;
    Email: string;
    Nickname: string;
    Picture: string;
    Age: number;
    Country: string;
    Genre: number;
    Language: number;
    Ethnicity: number;
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
        Country: string,
        Genre: number,
        Language: number,
        Ethnicity: number,
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
        this.Country = Country;
        this.Genre = Genre;
        this.Language = Language;
        this.Ethnicity = Ethnicity;
        this.ModifiedAt = new Date(ModifiedAt);
        this.RequesterId = RequesterId;
        this.IsProfileCompleted = IsProfileCompleted;
    }
}