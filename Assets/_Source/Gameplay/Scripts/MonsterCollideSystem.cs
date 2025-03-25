using System.Collections.Generic;
using System.Linq;
using EventBusSystem;
using UnityEngine;
using Zenject;

namespace Shmigliki.Gameplay
{
    public class MonsterCollideSystem : ILateTickable
    {
        private struct CollidePair
        {
            private Monster _monsterA;
            private Monster _monsterB;

            public CollidePair(Monster monsterA, Monster monsterB)
            {
                _monsterA = monsterA;
                _monsterB = monsterB;
            }

            public Monster MonsterA => _monsterA;
            public Monster MonsterB => _monsterB;

            public bool IsStillColliding(float additionalRadius)
            {
                var distanceCheck = Vector3.Distance(MonsterA.transform.position, MonsterB.transform.position) <= MonsterA.Size + MonsterB.Size + additionalRadius;
                var activeCheck = _monsterA.IsActive && _monsterB.IsActive;

                return distanceCheck && activeCheck;
            }
            
            public override bool Equals(object obj)
            {
                if (obj is not CollidePair)
                    return false;

                var otherPair = (CollidePair)obj;

                return (otherPair._monsterA == _monsterA || otherPair._monsterB == _monsterA) 
                        && (otherPair._monsterA == _monsterB || otherPair._monsterB == _monsterB);
            }
        }

        private float _additionalRadiousToCollide = 0.1f;
        private IMonsterHolder _monsterHolder;

        private List<CollidePair> _previouslyCollided = new List<CollidePair>();

        public MonsterCollideSystem(IMonsterHolder monsterHolder)
        {
            _monsterHolder = monsterHolder;
        }

        public void LateTick()
        {
            var collidedThisFrame = new List<CollidePair>();

            foreach (var monster in _monsterHolder.CurrentMonsters)
            {
                if (monster.IsActive == false)
                    continue;

                var collidingMonsters = _monsterHolder.CurrentMonsters
                    .Where(x => x.IsActive 
                                && Vector3.Distance(x.transform.position, monster.transform.position)
                                    < monster.Size + x.Size + 3f/*_additionalRadiousToCollide*/
                                && x != monster);

                foreach (var collidingMonster in collidingMonsters)
                {
                    var newColliderPair = new CollidePair(monster, collidingMonster);
                    
                    if (collidedThisFrame.Contains(newColliderPair) == false && _previouslyCollided.Contains(newColliderPair) == false)
                        collidedThisFrame.Add(newColliderPair);
                }
            }

            var pairsLoseCollision = new List<CollidePair>();
            foreach (var collidePair in _previouslyCollided)
                if (collidePair.IsStillColliding(_additionalRadiousToCollide) == false)
                    pairsLoseCollision.Add(collidePair);

            foreach (var collidePair in pairsLoseCollision)
                _previouslyCollided.Remove(collidePair);

            foreach (var collidePair in collidedThisFrame)
            {
                if (collidePair.IsStillColliding(_additionalRadiousToCollide))
                    EventBus.RaiseEvent<IMonsterCollisionHandler>(x => x.OnMonsterCollided(collidePair.MonsterA, collidePair.MonsterB));
            }
        }
    }
}