export class AppUser {
    id: string;
    name: string;
    email: string;
    age: number;
    country: string;
    genre: number;
    language: number;
    ethnicity: number;
    modifiedAt: Date;
    requesterId: string;

    constructor(
        id: string,
        name: string,
        email: string,
        age: number,
        country: string,
        genre: number,
        language: number,
        ethnicity: number,
        modifiedAt: Date,
        requesterId: string
    ) {
        this.id = id;
        this.name = name;
        this.email = email;
        this.age = age;
        this.country = country;
        this.genre = genre;
        this.language = language;
        this.ethnicity = ethnicity;
        this.modifiedAt = new Date(modifiedAt);
        this.requesterId = requesterId;
    }
}