using System;
using System.Collections.Generic;
using System.Text;

namespace BackendDungeonGen
{
    class Stack
    {
        private int pointer;
        private List<Room> stack;
        public Stack()
        {
            this.stack = new List<Room>();
            this.pointer = -1;
        }

        private bool isEmpty()
        {
            if (pointer == -1)
            {
                return true;
            }

            return false;
        }

        public bool pop()
        {
            if (isEmpty())
            {
                return false;
            }

            stack.RemoveAt(pointer);
            pointer -= 1;
            return true;
        }

        public bool push(Room newRoom)
        {
            stack.Add(newRoom);
            pointer += 1;
            return true;
        }

        public Room peek()
        {
            return stack[pointer];
        }
    }
}
