import axios from 'axios'
import { sha256 } from 'js-sha256'

axios.defaults.baseURL = ''

export const addDefaultHeader = (name: string, value: string) => {
    axios.defaults.headers.common[name] = value
}

export const removeDefaultHeader = (name: string) => {
    delete axios.defaults.headers.common[name]
}

export const login = (username: string, password: string) => {
    const passwordHash = sha256(password)
    return axios.post('/api/users/login', {username, password: passwordHash}).then(res => res.data)
}

export const register = (email: string, username: string, password: string) => {
    const passwordHash = sha256(password)
    return axios.post('/api/users/register', {email, username, password: passwordHash}).then(res => res.data)
}

export const sendMessage = (text: string, userId: number, date?: Date) => {
    return axios.post('/api/messages', {text, userId, clientSentAt: date || Date.now()}).then(res => res.data)
}

export const getMessages = (otherUserId: number) => {
    return axios.get(`/api/messages/${otherUserId}`).then(res => res.data)
}

export const getFriends = () => {
    return axios.get('/api/users/friends').then(res => res.data)
}