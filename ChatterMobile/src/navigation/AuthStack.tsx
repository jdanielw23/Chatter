import React from 'react'
import {createStackNavigator} from '@react-navigation/stack'
import { observer } from "mobx-react-lite";
import { useStore } from "../store/store";
import LoginScreen from '../screens/auth/LoginScreen';
import RegisterScreen from '../screens/auth/RegisterScreen';

export type AuthStackParamList = {
    Login: undefined
    Register: undefined
}

const Stack = createStackNavigator<AuthStackParamList>()
export default observer(() => {
    const store = useStore()
    return (
        <Stack.Navigator >
            <Stack.Screen name="Login" component={LoginScreen} />
            <Stack.Screen name="Register" component={RegisterScreen} />
        </Stack.Navigator>
    )
})