import { MessageStoreModel } from './models/message';
import { useContext, createContext } from "react"
import { types, Instance, onSnapshot } from "mobx-state-tree"
import { AuthModel } from "./models/auth"

const RootModel = types.model({
    auth: AuthModel,
    messages: MessageStoreModel
})

export const rootStore = RootModel.create({
    auth: {},
    messages: {}
})

onSnapshot(rootStore, snapshot => {
    console.log("Snapshot: ", snapshot)
})

export type RootInstance = Instance<typeof RootModel>
const RootStoreContext = createContext<null | RootInstance>(null)

export const Provider = RootStoreContext.Provider
export function useStore() {
    const store = useContext(RootStoreContext)
    if (store === null) {
        throw new Error("Store cannot be null")
    }
    return store as RootInstance
}