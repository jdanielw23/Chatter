import 'react-native-gesture-handler'
import React from 'react'
import AppNavigator from './src/navigation/AppNavigator'
import { Provider, rootStore } from './src/store/store'
import { ActionSheetProvider } from '@expo/react-native-action-sheet'
import { observer } from 'mobx-react-lite'

declare var global: { HermesInternal: null | {} }

const App = observer(() => {
    return (
        <ActionSheetProvider>
          <Provider value={rootStore}>
              <AppNavigator />
          </Provider>
        </ActionSheetProvider>
    )
})

export default App