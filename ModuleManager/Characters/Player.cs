using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class Player : Character
    {
        private CellModule _cellModule;
        private GuardModule _guardModule;
        private TeleportModule _teleportModule;
        private EventDispatcherModule _eventDispatcher;
        private SoundModule _soundModule;
        private CoinModule _coinModule;
        private StateModule _stateModule;
        private DiggingModule _diggingModule;

        public bool IsDigging { get; private set; }
        public CharacterAction LastMoveAction { get; set; }
        public bool GodMode { get; set; }

        public override void Initialize()
        {
            _cellModule = GetModule<CellModule>();
            _guardModule = GetModule<GuardModule>();
            _teleportModule = GetModule<TeleportModule>();
            _eventDispatcher = GetModule<EventDispatcherModule>();
            _soundModule = GetModule<SoundModule>();
            _coinModule = GetModule<CoinModule>();
            _stateModule = GetModule<StateModule>();
            _diggingModule = GetModule<DiggingModule>();
        }

        public virtual void Move()
        {
            if (!IsDigging)
            {
                var stayCurrPos = true;

                var curToken = _cellModule.Get(X, Y).Type;
                bool curState;
                Cell nextToken;
                if (curToken == CellType.Ladder || (curToken == CellType.RopeBar && YOffset == 0f))
                { //ladder & bar
                    curState = true; //ok to move (on ladder or bar)
                }
                else if (YOffset < 0)
                {  //no ladder && yOffset < 0 ==> falling 
                    curState = false;
                }
                else if (Y < _cellModule.Bottom)
                { //no laddr && y < maxTileY && yOffset >= 0

                    nextToken = _cellModule[X, Y + 1];
                    if (nextToken.IsEmpty)
                    {
                        curState = false;
                    }
                    else if (nextToken.Any(CellType.Block, CellType.Ladder, CellType.Solid))
                    {
                        curState = true;
                    }
                    else if (_guardModule.IsGuardAt(X, Y + 1))
                    {
                        curState = true;
                    }
                    else
                    {
                        curState = false;
                    }

                }
                else
                { // no laddr && y == maxTileY 
                    curState = true;
                }

                if (!curState)
                {
                    stayCurrPos = (Y >= _cellModule.Bottom ||
                        _cellModule[X, Y + 1].Any(CellType.Block, CellType.Solid) ||
                        _guardModule.IsGuardAt(X, Y + 1));

                    MoveStep(CharacterAction.Fall, stayCurrPos);
                    return;
                }
            }
        }

        public void MoveStep(CharacterAction action, bool stayCurrPos)
        {
            var x = X;
            var xOffset = XOffset;
            var y = Y;
            var yOffset = YOffset;

            //var curShape = _runner.shape;
            //var newShape = curShape;

            var centerX = CharacterAction.Stop;
            var centerY = centerX;

            switch (action)
            {
                case CharacterAction.DigLeft:
                case CharacterAction.DigRight:
                    xOffset = 0;
                    yOffset = 0;
                    break;
                case CharacterAction.Up:
                case CharacterAction.Down:
                case CharacterAction.Fall:
                    if (xOffset > 0) centerX = CharacterAction.Left;
                    else if (xOffset < 0) centerX = CharacterAction.Right;
                    break;
                case CharacterAction.Left:
                case CharacterAction.Right:
                    if (yOffset > 0) centerY = CharacterAction.Up;
                    else if (yOffset < 0) centerY = CharacterAction.Down;
                    break;
            }

            var curToken = _cellModule[x, y].Type;

            if (action == CharacterAction.Up)
            {
                yOffset -= GetModifiedValue(CharacterProperty.YMove);//_runner.yMove;

                if (stayCurrPos && yOffset < 0) yOffset = 0; //stay on current position
                else if (yOffset < -_cellModule.H2)
                { //move to y-1 position 
                    //if (curToken == CellType.Block || curToken == CellType.HLadr) curToken = CellType.Empty; //in hole or hide laddr
                    //map[x, y].Act = curToken; //runner move to [x][y-1], so set [x][y].act to previous state
                    y--;
                    yOffset = _cellModule.TileH + yOffset;
                    //if (map[x, y].Act == CubePlatformType.Guard && AI.Guard.guardAlive(x, y)) setRunnerDead(); //collision
                    if (_guardModule.IsGuardAlive(x, y)) Dead();
                }
                //newShape = "RunUpDn";
            }

            if (centerY == CharacterAction.Up)
            {
                yOffset -= GetModifiedValue(CharacterProperty.YMove);
                if (yOffset < 0) yOffset = 0; //move to center Y	
            }

            if (action == CharacterAction.Down || action == CharacterAction.Fall)
            {
                var holdOnBar = 0;
                if (curToken == CellType.RopeBar)
                {
                    if (yOffset < 0) holdOnBar = 1;
                    else if (action == CharacterAction.Down && y < _cellModule.Bottom && _cellModule[x, y + 1].Type != CellType.Ladder)
                    {
                        action = CharacterAction.Fall; //shape fixed: 2014/03/27
                    }
                }

                yOffset += GetModifiedValue(CharacterProperty.YMove);

                if (holdOnBar == 1 && yOffset >= 0)
                {
                    yOffset = 0; //fall and hold on bar
                    action = CharacterAction.FallBar;
                }
                if (stayCurrPos && yOffset > 0) yOffset = 0; //stay on current position
                else if (yOffset > _cellModule.H2)
                { //move to y+1 position
                    //if (curToken == CubePlatformType.Block || curToken == CubePlatformType.HLadr) curToken = CubePlatformType.Empty; //in hole or hide laddr
                    //map[x][y].Act = curToken; //runner move to [x][y+1], so set [x][y].act to previous state
                    y++;
                    yOffset = yOffset - _cellModule.TileH;
                    if (_guardModule.IsGuardAlive(x, y)) Dead(); //collision
                }

                if (action == CharacterAction.Down)
                {
                    //newShape = "RunUpDn";
                    _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.RunUpDn));
                }
                else
                { //ACT_FALL or ACT_FALL_BAR

                    if (y < _cellModule.Bottom && _guardModule.IsGuardAt(x, y + 1))
                    { //over guard
                        //don't collision
                        var guard = _guardModule.GetGuardAt(x, y + 1);//AI.Guard.getGuardId(x, y + 1);
                        if (yOffset > guard.YOffset) yOffset = guard.YOffset;
                    }

                    if (action == CharacterAction.FallBar)
                    {
                        if (LastMoveAction == CharacterAction.Left)
                        {
                            _eventDispatcher.DispatchEvent(
                                new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.BarLeft));
                            //newShape = "BarLeft");
                        }
                        else
                        {
                            _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.BarRight));
                            //newShape = "BarRight";
                        }
                    }
                    else
                    {
                        if (LastMoveAction == CharacterAction.Left)
                        {
                            _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.FallLeft));
                            //newShape = "FallLeft";
                        }
                        else
                        {
                            _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.FallRight));
                        }
                        //else newShape = "FallRight";
                    }
                }
            }

            if (centerY == CharacterAction.Down)
            {
                yOffset += GetModifiedValue(CharacterProperty.YMove);
                if (yOffset > 0) yOffset = 0; //move to center Y
            }

            if (action == CharacterAction.Left)
            {
                xOffset -= GetModifiedValue(CharacterProperty.XMove);

                if (stayCurrPos && xOffset < 0) xOffset = 0; //stay on current position
                else if (xOffset < -_cellModule.W2)
                { //move to x-1 position 
                    //if (curToken == CubePlatformType.Block || curToken == CubePlatformType.HLadr) curToken = CubePlatformType.Empty; //in hole or hide laddr
                    //map[x][y].Act = curToken; //runner move to [x-1][y], so set [x][y].act to previous state
                    x--;
                    xOffset = _cellModule.TileW + xOffset;
                    if (_guardModule.IsGuardAlive(x, y)) Dead(); //collision
                }
                if (curToken == CellType.RopeBar)
                {
                    _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.BarLeft));
                    //newShape = "BarLeft";
                }
                else
                {
                    _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.RunLeft));
                    //else newShape = "RunLeft";
                }
            }

            if (centerX == CharacterAction.Left)
            {
                xOffset -= GetModifiedValue(CharacterProperty.XMove);
                if (xOffset < 0) xOffset = 0; //move to center X
            }

            if (action == CharacterAction.Right)
            {
                xOffset += GetModifiedValue(CharacterProperty.XMove);

                if (stayCurrPos && xOffset > 0) xOffset = 0; //stay on current position
                else if (xOffset > _cellModule.W2)
                { //move to x+1 position 
                    //if (curToken == CubePlatformType.Block || curToken == CubePlatformType.HLadr) curToken = CubePlatformType.Empty; //in hole or hide laddr
                    //map[x][y].Act = curToken; //runner move to [x+1][y], so set [x][y].act to previous state
                    x++;
                    xOffset = xOffset - _cellModule.TileW;
                    if (_guardModule.IsGuardAlive(x, y)) Dead(); //collision
                }
                if (curToken == CellType.RopeBar)
                {
                    _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.BarRight));
                    //newShape = "BarRight";
                }
                else
                {
                    _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.RunRight));
                    //else newShape = "runRight";
                }
            }

            if (centerX == CharacterAction.Right)
            {
                xOffset += GetModifiedValue(CharacterProperty.XMove);
                if (xOffset > 0) xOffset = 0; //move to center X
            }

            if (action == CharacterAction.Stop)
            {
                if (Action == CharacterAction.Fall)
                {
                    _soundModule.StopFall();
                    _soundModule.PlayDown();
                }
                if (Action != CharacterAction.Stop)
                {
                    //_runner.sprite.stop();
                    _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.Stop));
                    Action = CharacterAction.Stop;
                }
            }
            else
            {
                //_runner.sprite.x = (x + xOffset);
                //_runner.sprite.y = (y + yOffset);
                SetPosition(x, y, xOffset, yOffset);
                //if (curShape != newShape)
                //{
                //_runner.sprite.gotoAndPlay(newShape);
                //_runner.shape = newShape;
                //}
                if (action != Action)
                {
                    if (Action == CharacterAction.Fall)
                    {
                        _soundModule.StopFall();
                        _soundModule.PlayDown();
                    }
                    else if (action == CharacterAction.Fall)
                    {
                        _soundModule.PlayFall();
                    }
                    //_runner.sprite.play();
                }
                if (action == CharacterAction.Left || action == CharacterAction.Right) LastMoveAction = action;
                Action = action;
            }
            //map[x][y].Act = CubePlatformType.Runner;

            // Check runner to get gold (MAX MOVE MUST < H4 & W4) 
            if (_coinModule.IsCoin(x, y) &&
                ((xOffset == 0f && yOffset >= 0 && yOffset < _cellModule.H4) ||
                 (yOffset == 0f && xOffset >= 0 && xOffset < _cellModule.W4) ||
                 (y < _cellModule.Bottom && _cellModule.Get(x, y + 1).Type == CellType.Ladder && yOffset < _cellModule.H4) // gold above laddr
                )
              )
            {
                _coinModule.RemoveAt(x, y);
                _soundModule.PlayGetGold();
                //debug("gold = " + goldCount);
                //if (playMode == PLAY_CLASSIC || playMode == PLAY_AUTO || playMode == PLAY_DEMO)
                //{
                //    drawScore(SCORE_GET_GOLD);
                //}
                //else
                //{
                //for modern mode , edit mode
                //AI.drawGold(1); //get gold 
                //}
            }
            if (_coinModule.IsEmpty && !_coinModule.IsCompleted) _cellModule.ShowHiddenLadder();

            //check collision with guard !
            СheckCollision(x, y);
        }

        public void СheckCollision(int runnerX, int runnerY)
        {
            var x = -1;
            var y = -1;
            //var dbg = "NO";

            if (runnerY > 0 && _guardModule.IsGuardAlive(runnerX, runnerY - 1))
            {
                x = runnerX;
                y = runnerY - 1;
                //dbg = "UP";	
            }
            else if (runnerY < _cellModule.Bottom && _guardModule.IsGuardAlive(runnerX, runnerY + 1))
            {
                x = runnerX;
                y = runnerY + 1;
                //dbg = "DN";	
            }
            else if (runnerX > 0 && _guardModule.IsGuardAlive(runnerX - 1, runnerY))
            {
                x = runnerX - 1;
                y = runnerY;
                //dbg = "LF";	
            }
            else if (runnerX < _cellModule.Right && _guardModule.IsGuardAlive(runnerX + 1, runnerY))
            {
                x = runnerX + 1;
                y = runnerY;
                //dbg = "RT";	
            }

            //if( dbg != "NO") debug(dbg);
            if (x >= 0)
            {
                var guard = _guardModule.GetGuardAt(x, y);
                if (guard.Action != CharacterAction.Reborn)
                { //only guard alive need check collection
                    //var dw = Math.abs(runner.sprite.x - guard[i].sprite.x);
                    //var dh = Math.abs(runner.sprite.y - guard[i].sprite.y);

                    //change detect method ==> don't depend on scale 
                    var runnerPosX = X * _cellModule.TileW + XOffset;
                    var runnerPosY = Y * _cellModule.TileH + YOffset;
                    var guardPosX = guard.X * _cellModule.TileW + guard.XOffset;
                    var guardPosY = guard.Y * _cellModule.TileH + guard.YOffset;

                    var dw = Math.Abs(runnerPosX - guardPosX);
                    var dh = Math.Abs(runnerPosY - guardPosY);

                    if (dw <= _cellModule.W4 * 3 && dh <= _cellModule.H4 * 3)
                    {
                        Dead(); //07/04/2014
                        //debug("runner dead!");
                    }
                }
            }
        }

        private void Dead()
        {
            if (!GodMode)
            {
                _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.Dead));
                _stateModule.State = State.Dead;
            }
        }

        public virtual void NextInput(Input input)
        {
            switch (input.Type)
            {
                case InputType.Move:
                    InputMove(input.Action);
                    break;
                case InputType.Skill:
                    break;
            }
        }

        protected virtual void InputMove(InputAction action)
        {
            var moveStep = CharacterAction.Stop;
            var stayCurrPos = true;
            var keyAction = action;
            Cell nextToken;
            switch (keyAction)
            {
                case InputAction.MoveUp:
                    stayCurrPos = Y <= 0 || (nextToken = _cellModule.Get(X, Y - 1)).IsNotHiddenBlock || nextToken.Any(CellType.Solid, CellType.Trap);

                    if (Y > 0 &&
                        _cellModule.Get(X, Y).Type != CellType.Ladder &&
                        YOffset < _cellModule.H4 &&
                        YOffset > 0 &&
                        _cellModule.Get(X, Y + 1).Type == CellType.Ladder)
                    {
                        stayCurrPos = true;
                        moveStep = CharacterAction.Up;
                    }
                    else if (!(_cellModule.Get(X, Y).Type != CellType.Ladder &&
                              (YOffset <= 0 || _cellModule.Get(X, Y + 1).Type != CellType.Ladder) ||
                              (YOffset <= 0 && stayCurrPos))
                            )
                    {
                        moveStep = CharacterAction.Up;
                    }

                    break;
                case InputAction.MoveDown:
                    stayCurrPos = Y >= _cellModule.Bottom ||
                                  (nextToken = _cellModule.Get(X, Y + 1)).IsNotHiddenBlock ||
                                  nextToken.Type == CellType.Solid;

                    if (!(YOffset >= 0 && stayCurrPos))
                    {
                        moveStep = CharacterAction.Down;
                    }
                    break;
                case InputAction.MoveLeft:
                    stayCurrPos = X <= 0 ||
                                  (nextToken = _cellModule.Get(X - 1, Y)).IsNotHiddenBlock ||
                                  nextToken.Any(CellType.Solid, CellType.Trap);

                    if (!(XOffset <= 0 && stayCurrPos))
                    {
                        moveStep = CharacterAction.Left;
                    }
                    break;
                case InputAction.MoveRight:
                    stayCurrPos = X >= _cellModule.Right ||
                                  (nextToken = _cellModule.Get(X + 1, Y)).IsNotHiddenBlock ||
                                  nextToken.Any(CellType.Solid, CellType.Trap);

                    if (!(XOffset >= 0 && stayCurrPos))
                        moveStep = CharacterAction.Right;
                    break;
                case InputAction.DigLeft:
                case InputAction.DigRight:
                    CharacterAction characterAction;
                    if(keyAction == InputAction.DigLeft) characterAction = CharacterAction.Left;
                    else characterAction = CharacterAction.Right;
                    
                    if (_diggingModule.AvailableToDigg(characterAction))
                    {
                        MoveStep(characterAction, stayCurrPos);
                        Digging(characterAction);
                    }
                    else
                    {
                        MoveStep(CharacterAction.Stop, stayCurrPos);
                    }
                    return;
            }
            MoveStep(moveStep, stayCurrPos);
        }

        protected virtual void Digging(CharacterAction action)
        {
            int x;
            int y;
            //string holeShape;

            if (action == CharacterAction.DigLeft)
            {
                x = X - 1;
                y = Y;

                //_runner.shape = CubeAction.DigLeft;
                //holeShape = "digHoleLeft";
                _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.DigLeft));
            }
            else
            { //DIG RIGHT

                x = X + 1;
                y = Y;

                //_runner.shape = CubeAction.DigRight;
                //holeShape = "digHoleRight";
                _eventDispatcher.DispatchEvent(new PlayerAnimationEvent(PlayerAnimationEvent.AnimationEvent.DigRight));
            }

            _soundModule.PlayDig();
            _eventDispatcher.DispatchEvent(new DiggingCellEvent(x, y + 1));
            //AI.Map[x, y + 1].HideBlock(); //hide block (replace with digging image)
            //_runner.sprite.gotoAndPlay(_runner.shape);

            /*var holeObj = AI.HoleObj;
            holeObj.action = CubeAction.Digging;
            holeObj.pos.Set(x, y);
            holeObj.sprite.setTransform(x, y);
            holeObj.StartDigging(AI.Map[x, y + 1]);
            holeObj.DigEnd += digComplete;

            //digTimeStart = recordCount; //for debug

            holeObj.sprite.gotoAndPlay(_runner.shape);
            holeObj.sprite.onComplete(digComplete);
            holeObj.AddToScene();*/
        }
    }
}
