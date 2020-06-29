using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Alex.API.Input.Listeners
{
    public abstract class InputListenerBase<TState, TButtons> : IInputListener,
        IEnumerable<KeyValuePair<InputCommand, List<TButtons>>>
    {
        public PlayerIndex PlayerIndex { get; }

        private readonly IDictionary<InputCommand, List<TButtons>> _buttonMap =
            new Dictionary<InputCommand, List<TButtons>>();

        protected TState PreviousState, CurrentState;

        protected abstract TState GetCurrentState();

        protected abstract bool IsButtonDown(TState state, TButtons buttons);
        protected abstract bool IsButtonUp(TState state, TButtons buttons);

        protected InputListenerBase(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public void Update(GameTime gameTime)
        {
            PreviousState = CurrentState;
            CurrentState = GetCurrentState();

            OnUpdate(gameTime);
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
        }

        public void RegisterMap(InputCommand command, TButtons buttons)
        {
            if (!_buttonMap.ContainsKey(command))
            {
                _buttonMap.Add(command, new List<TButtons>());
            }

            if (!_buttonMap[command].Contains(buttons))
            {
                _buttonMap[command].Add(buttons);
            }
        }

        public void RemoveMap(InputCommand command)
        {
            if (_buttonMap.ContainsKey(command))
                _buttonMap.Remove(command);
        }

        public bool IsDown(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && buttons.Any(btn => IsButtonDown(CurrentState, btn)));
        }

        public bool IsUp(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && buttons.Any(btn => IsButtonUp(CurrentState, btn)));
        }

        public bool IsBeginPress(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && buttons.Any(btn => IsButtonDown(CurrentState, btn) &&
                                                                                  IsButtonUp(PreviousState, btn)));
        }

        public bool IsPressed(InputCommand command)
        {
            return (TryGetButtons(command, out var buttons) && buttons.Any(btn => IsButtonUp(CurrentState, btn) &&
                                                                                  IsButtonDown(PreviousState, btn)));
        }

        private bool TryGetButtons(InputCommand command, out IReadOnlyCollection<TButtons> buttons)
        {
            if (_buttonMap.TryGetValue(command, out var val))
            {
                buttons = val;
                return true;
            }

            buttons = new TButtons[0];
            return false;
        }

        public IEnumerator<KeyValuePair<InputCommand, List<TButtons>>> GetEnumerator()
        {
            foreach (var kv in _buttonMap)
                yield return kv;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}