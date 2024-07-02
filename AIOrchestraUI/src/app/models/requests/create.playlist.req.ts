export class CreatePlaylistReq {
    constructor(
        public name: string,
        public userId: string,
        public songIds: string[]
    ) { }
}