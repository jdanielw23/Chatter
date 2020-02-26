import { types, flow } from 'mobx-state-tree'
import * as http from '../../common/http' 

export const AuthModel = types.model({
    userId: types.maybe(types.number),
    username: types.maybe(types.string),
    token: types.maybe(types.string)
}).views(self => ({
    get isLoggedIn() {
        return self.token !== undefined
    }
})).actions(self => ({
    login: flow(function*(username, password) {
        const res = yield http.login(username, password)
        self.userId = res.user.id
        self.username = username
        self.token = res.token
        http.addDefaultHeader('Authorization', res.token)
    }),
    register: flow(function*(email, username, password){
        const res = yield http.register(email, username, password)
        self.userId = res.user.id
        self.username = username
        self.token = res.token
        http.addDefaultHeader('Authorization', res.token)
    }),
    logout: () => {
      self.userId = undefined
      self.username = undefined
      self.token = undefined
      http.removeDefaultHeader('Authorization')
    }
}))

