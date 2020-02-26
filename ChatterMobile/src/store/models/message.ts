import { UserModel } from './user';
import { types, flow } from 'mobx-state-tree'
import * as http from '../../common/http' 

export const MessageModel = types.model({
    _id: types.maybe(types.number),
    text: types.maybe(types.string),
    user: types.maybe(UserModel),
    createdAt: types.Date,
    sent: types.maybe(types.boolean),
    received: types.maybe(types.boolean)
}).views(self => ({
})).actions(self => ({
}))

export const MessageStoreModel = types.model({
    all: types.array(MessageModel)
}).actions(self => ({
    load: flow(function*(otherUserId) {
        const messages = yield http.getMessages(otherUserId)
        self.all = messages.map(m => ({
            _id: m.id,
            text: m.text,
            createdAt: m.sentAt,
            sent: m.deliveredAt !== undefined,
            received: m.readAt !== undefined,
            user: {
                _id: m.toUser.id,
                name: m.toUser.username
            }
        }))
    })
}))

