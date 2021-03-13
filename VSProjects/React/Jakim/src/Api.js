import {AsyncStorage} from 'react-native';


const BASE_API = 'http://192.168.1.67:80/api';

export default {
    checkToken: async (token) => {
        /*const req = await fetch(`${BASE_API}/utilizadores`, {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({token})
        });
        const json = await req.json();
        return json;*/
    },
    signIn: async (email, password) => {
        const req = await fetch(`${BASE_API}/utilizadores/login`, {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({email, password})
        });
        const json = await req.json();
        return json;
    },
    signUp: async (name, nif, email, password) => {
        const req = await fetch(`${BASE_API}/utilizadores/registar`, {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({name, nif, email, password })
        });
        const json = await req.json();
        return json;
    },
    getReservas : async (freguesiaNome, inicio, fim) => {
        const token = await AsyncStorage.getItem('token');
        const req = await fetch(`${BASE_API}/reservas/disponibilidade/${freguesiaNome}/${inicio}/${fim}`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        });
        const json = await req.json();
        return json;
    },
    getSaldo : async () => {
        const token = await AsyncStorage.getItem('token');
        const nif = await AsyncStorage.getItem('nif');
        const req = await fetch(`${BASE_API}/utilizadores/saldo/${nif}`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        });
        const json = await req.json();
        return json;
    },
    addValor : async (valor) => {
        const token = await AsyncStorage.getItem('token');
        const nif = await AsyncStorage.getItem('nif');
        const req = await fetch(`${BASE_API}/utilizadores/depositar`, {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify({'nif':nif, 'valor':valor})
        });
        const json = await req.json();
        return json;
    },
    getTransacao : async () => {
        const token = await AsyncStorage.getItem('token');
        const nif = await AsyncStorage.getItem('nif');
        const req = await fetch(`${BASE_API}/transacoes/all/${nif}`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        });
        const json = await req.json();
        return json;
    },
};