import { User } from "./user";

export interface Profile {
    username: string;
    displayName: string;
    imageUrl?: string;
    bio?: string;
}

export class Profile implements Profile {
    constructor(user: User) {
        this.username = user.username;
        this.displayName = user.displayName;
        this.imageUrl = user.imageUrl;
    }
}