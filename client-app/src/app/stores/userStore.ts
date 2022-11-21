import { makeAutoObservable, runInAction } from "mobx";
import { history } from "../..";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";
import { store } from "./store";

export default class UserStore {
    currentUser: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() {
        return !!this.currentUser;
    }

    loginUser = async (creds: UserFormValues) => {
        try {
            var user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.currentUser = user);
            history.push('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

    logoutUser = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.currentUser = null;
        history.push('/');
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            runInAction(() => this.currentUser = user);
        } catch (error) {
            console.log(error);
        }
    }

    registerUser = async (creds: UserFormValues) => {
        try {
            var user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.currentUser = user);
            history.push('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }
}