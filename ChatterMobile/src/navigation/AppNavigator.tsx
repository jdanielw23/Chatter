import React from 'react'
import { NavigationContainer } from '@react-navigation/native'
import { observer } from 'mobx-react-lite'
import { useStore } from '../store/store'
import AuthStack from './AuthStack'
import MessagingStack from './MessagingStack'

export default observer(() => {
    const store = useStore()
    return (
        <NavigationContainer>
            {store.auth.isLoggedIn ? <MessagingStack /> : <AuthStack />}
        </NavigationContainer>
    )
})
