import { types, flow } from 'mobx-state-tree'
import * as http from '../../common/http'

export const UserModel = types.model({
    _id: types.maybe(types.number),
    name: types.maybe(types.string),
}).views(self => ({
})).actions(self => ({
}))

export const FriendsModel = types.model({
    all: types.array(UserModel)
}).actions(self => ({
    load: flow(function*() {
        const friends = yield http.getFriends()
        self.all = friends.map(f => ({
            _id: f.id,
            name: f.username
        }))
    })
}))